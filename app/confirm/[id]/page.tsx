"use client";

import { createFinalLoan, getLoanByUserID, updateLoan } from "@/app/services/moneyme";
import { FormControl, FormField, FormItem, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { zodResolver } from "@hookform/resolvers/zod";
import { differenceInYears } from "date-fns";
import React, { useEffect, useState } from "react";
import DatePicker from "react-datepicker";
import { FormProvider, useForm } from "react-hook-form";
import { date, z } from "zod";
import "react-datepicker/dist/react-datepicker.css";
import { Slider } from "@/components/ui/slider";
import { Button } from "@/components/ui/button";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { toast, ToastContainer } from "react-toastify";

import "react-toastify/dist/ReactToastify.css";

const FormSchema = z.object({
  product: z.string().min(2, { message: "Please select a product." }),
  title: z.string({ required_error: "Please select a title." }),
  firstName: z.string().min(2, { message: "First name must be at least 2 characters." }),
  lastName: z.string().min(2, { message: "Last name must be at least 2 characters." }),
  email: z.string().email({ message: "Invalid email address." }),
  mobile: z.string().min(10, { message: "Mobile number must be at least 10 digits." }),
  dateOfBirth: z.date().refine((date) => differenceInYears(new Date(), date) >= 18, {
    message: "You must be at least 18 years old.",
  }),
  amountRequired: z.number().min(100, { message: "Loan amount must be at least $2,100." }),
  term: z.number().min(1, { message: "Select a term" }),
});

function Confirm(context: any) {
  const { params } = context;
  const id = params.id;

  const [loading, setLoading] = useState<boolean>(true);
  const [isEditing, setIsEditing] = useState<boolean>(false);

  const form = useForm<z.infer<typeof FormSchema>>({
    resolver: zodResolver(FormSchema),
    defaultValues: {
      title: "",
      product: "",
      firstName: "",
      lastName: "",
      email: "",
      mobile: "",
      dateOfBirth: undefined,
      amountRequired: 100,
      term: 1,
    },
  });
 
  const firstName = form.watch("firstName");
  const lastName = form.watch("lastName");
  const phoneNumber = form.watch("mobile");
  const email = form.watch("email");
  const dateOfBirth = form.watch("dateOfBirth");
  const amountRequired = form.watch("amountRequired");
  const term = form.watch("term");
  const product = form.watch("product");
  const title = form.watch("title");

  const [repaymentsFrom, setRepaymentsFrom] = useState<number>(0);

  useEffect(() => {
    async function getLoan() {
      try {
        const response = await getLoanByUserID(id);
        const result = response.result;

        form.setValue("firstName", result.firstName);
        form.setValue("lastName", result.lastName);
        form.setValue("mobile", result.phoneNumber.slice(3));
        form.setValue("email", result.email);
        form.setValue("dateOfBirth", result.dateOfBirth);
        form.setValue("product", result.product);
        form.setValue("title", result.title);
        form.setValue("amountRequired", Number(result.amountRequired));   
        form.setValue("term", Number(result.term));  
        setRepaymentsFrom(result.repaymentsFrom);

        setLoading(false);
      } catch (error) {
        console.error("Error fetching loan data:", error);
        setLoading(false);
      }
    }
    getLoan();
  }, [id, form.setValue]);

  const handleEditClick = () => {
    setIsEditing(true);
  };

  const  handleApplyNow = async () => {
    const newData = {
        loanAmount: amountRequired ,
        term: term,
        title: title,
        firstName: firstName,
        lastName: lastName,
        dateOfBirth: dateOfBirth,
        phoneNumber: `+63${phoneNumber}`,
        email: email,
        product: product,
        repayment: repaymentsFrom

    }
    console.log(JSON.stringify(newData))
    const response = await createFinalLoan(newData, id)

    if(response.statusCode === 201) {
      window.location.href = 'https://www.moneyme.com.au/';
    }
  }

  async function onSubmitEdit(data: z.infer<typeof FormSchema>) {
      const newData ={
        product: data.product,
        term: data.term.toString(),
        title: data.title,
        firstName: data.firstName,
        lastName: data.lastName,
        email: data.email,
        phoneNumber: `+63${data.mobile}`,
        dateOfBirth: data.dateOfBirth,
        amountRequired: data.amountRequired
      }
      console.log(JSON.stringify(newData))
      const response = await createFinalLoan(newData, id)

      if(response.statusCode === 200) {
        toast("Successfully updated your loan details.")
      }


  }
  function onError(errors: any) {
    console.log("Form validation errors:", errors);
  }
  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-100">
      <div className="bg-white p-10 rounded-lg shadow-md w-full max-w-lg">
        <h1 className="text-center text-2xl font-bold mb-6">Your quote</h1>

        {loading ? (
          <div>
            <div className="mb-6">
              <div className="flex justify-between items-center">
                <h2 className="font-semibold text-lg">Your information</h2>
                <span className="w-10 h-4 bg-gray-300 animate-pulse rounded"></span>
              </div>
              <div className="mt-2 space-y-2">
                <div className="h-4 bg-gray-300 animate-pulse rounded"></div>
                <div className="h-4 bg-gray-300 animate-pulse rounded"></div>
                <div className="h-4 bg-gray-300 animate-pulse rounded"></div>
              </div>
            </div>
            <div className="mb-6">
              <div className="flex justify-between items-center">
                <h2 className="font-semibold text-lg">Finance details</h2>
                <span className="w-10 h-4 bg-gray-300 animate-pulse rounded"></span>
              </div>
              <div className="mt-2 space-y-2">
                <div className="h-4 bg-gray-300 animate-pulse rounded"></div>
                <div className="h-4 bg-gray-300 animate-pulse rounded"></div>
              </div>
            </div>

            <div className="flex justify-center">
              <div className="w-32 h-10 bg-gray-300 animate-pulse rounded"></div>
            </div>
          </div>
        ) : (
          <FormProvider {...form}>
            <form onSubmit={form.handleSubmit(onSubmitEdit, onError)}>
              <div className="mb-6">
                <div className="flex justify-between items-center">
                  <h2 className="font-semibold text-lg">Your information</h2>
                  {isEditing ? (
                  <>
                  <div className="flex space-x-2">
                    <Button
                        type="submit"
                        className="bg-green-500 text-white font-bold py-1 px-3 rounded hover:bg-green-600"
                        >
                      Save
                    </Button>
                    <Button onClick={(e) => setIsEditing(false)}
                      className="bg-red-500 text-white font-bold py-1 px-3 rounded hover:bg-red-600"
                    >
                      Cancel
                    </Button>
                  </div>
                </>
                  ) : (
                    <Button onClick={handleEditClick}  className="bg-green-500 text-white font-bold py-1 px-3 rounded hover:bg-green-600">
                      Edit
                    </Button>
                  )}
                </div>

                <div className="mt-2 text-gray-700 space-y-1">
                  {isEditing ? (
                    <>
                     <FormField
                        control={form.control}
                        name="title"
                        render={({ field }) => (
                          <FormItem className="flex-1">
                            <Select onValueChange={field.onChange} defaultValue={field.value}>
                              <FormControl>
                                <SelectTrigger>
                                  <SelectValue placeholder="Select Title" />
                                </SelectTrigger>
                              </FormControl>
                              <SelectContent>
                                <SelectItem value="Mr">Mr.</SelectItem>
                                <SelectItem value="Mrs">Mrs.</SelectItem>
                                <SelectItem value="Ms">Ms.</SelectItem>
                              </SelectContent>
                            </Select>
                            <FormMessage />
                          </FormItem>
                        )}
                      />

                      <FormField
                        control={form.control}
                        name="firstName"
                        render={({ field }) => (
                          <FormItem>
                            <FormControl>
                              <Input placeholder="First name" {...field} />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                      <FormField
                        control={form.control}
                        name="lastName"
                        render={({ field }) => (
                          <FormItem>
                            <FormControl>
                              <Input placeholder="Last name" {...field} />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />

                      <FormField
                        control={form.control}
                        name="mobile"
                        render={({ field }) => (
                          <FormItem>
                            <FormControl>
                              <Input placeholder="Phone number" {...field} />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />

                      <FormField
                        control={form.control}
                        name="email"
                        render={({ field }) => (
                          <FormItem>
                            <FormControl>
                              <Input placeholder="Email" {...field} />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />

                      <FormField
                        control={form.control}
                        name="dateOfBirth"
                        render={({ field }) => (
                          <FormItem>
                            <FormControl>
                              <DatePicker
                                selected={field.value}
                                onChange={(date) => {
                                  field.onChange(date);
                                }}
                                showMonthDropdown
                                showYearDropdown
                                dropdownMode="select"
                                placeholderText="Date of Birth"
                                className="w-full px-3 py-2 border rounded-md"
                                dateFormat="MM/dd/yyyy"
                              />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                    </>
                  ) : (
                    <>
                    <div className="flex justify-between items-center">
                    <span>Name:</span>
                        <span className="text-md  text-green-600">
                        {title} {firstName} {lastName}
                        </span>
                    </div>
                    <div className="flex justify-between items-center">
                    <span>Phone:</span>
                        <span className="text-md   text-green-600">
                        {phoneNumber}
                        </span>
                    </div>
                    <div className="flex justify-between items-center">
                    <span>Email:</span>
                        <span className="text-md text-green-600">
                        {email}
                        </span>
                    </div>
                    <div className="flex justify-between items-center">
                    <span>Date of Birth: :</span>
                        <span className="text-md text-green-600">
                        {new Date(dateOfBirth).toLocaleDateString()}
                        </span>
                    </div>
                    </>
                  )}
                </div>
              </div>

              <div className="mb-6">
                <div className="flex justify-between items-center">
                  <h2 className="font-semibold text-lg">Finance details</h2>
                </div>
                <div className="mt-2 text-gray-700">
                  {isEditing ? (
                         <>
                          <div className="mb-5">
                          <FormField
                            control={form.control}
                            name="product"
                            render={({ field }) => (
                              <FormItem className="flex-1">
                                <Select onValueChange={field.onChange} defaultValue={field.value}>
                                  <FormControl>
                                    <SelectTrigger>
                                      <SelectValue placeholder="Select Product" />
                                    </SelectTrigger>
                                  </FormControl>
                                  <SelectContent>
                                    <SelectItem value="ProductA">ProductA</SelectItem>
                                    <SelectItem value="ProductB">ProductB</SelectItem>
                                    <SelectItem value="ProductC">ProductC</SelectItem>
                                  </SelectContent>
                                </Select>
                                <FormMessage />
                              </FormItem>
                            )}
                          />
                          </div>
                         <label>Amount</label>
                         <Slider
                           value={[form.watch('amountRequired')]}   
                           min={100}
                           max={15000}
                           step={100}
                           onValueChange={(value) => form.setValue("amountRequired", value[0])}   
                         />
                         <div className="flex justify-between text-gray-600 mt-2">
                           <span>$100</span>
                           <span className="font-semibold">${form.watch('amountRequired')}</span>  
                           <span>$15,000</span>
                         </div>
                     
                         <label>Term</label>
                         <Slider
                           value={[form.watch('term')]}   
                           min={1}
                           max={12}
                           step={1}
                           onValueChange={(value) => form.setValue("term", value[0])}   
                         />
                         <div className="flex justify-between text-gray-600 mt-2">
                           <span>1</span>
                           <span className="font-semibold">{form.watch('term')} month</span> 
                           <span>12</span>
                         </div>
                       </>
                  ) : (
                    <>
                     <p>
                        Product :{" "}
                        <span className="text-md text-green-600">
                         {product}
                        </span>{" "}
                      </p>

                      <p>
                        Finance amount:{" "}
                        <span className="text-md  text-green-600">
                          ${amountRequired}
                        </span>{" "}
                        over {term} months
                      </p>
                      <p>
                        Repayments from:{" "}
                        <span className="text-md  text-green-600">
                          ${repaymentsFrom.toFixed(2)}
                        </span>{" "}
                        Monthly
                      </p>
                    </>
                  )}
                </div>
              </div>

             
            </form>
          </FormProvider>
        )}
       
        {!isEditing && (
           <div className="flex justify-center">
           <button onClick={handleApplyNow} className="bg-green-500 text-white font-bold py-3 px-6 rounded hover:bg-green-600">
             Apply now
           </button>
         </div>
        )}

        
      </div>

      <ToastContainer />

    </div>
  );
}

export default Confirm;

"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { useState } from "react";
import { differenceInYears } from "date-fns";
import DatePicker from "react-datepicker";

import { Button } from "@/components/ui/button";
import { Slider } from "@/components/ui/slider";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Select, SelectTrigger, SelectContent, SelectItem, SelectValue } from "@/components/ui/select";
import { RefreshCwIcon } from "lucide-react";
import { useRouter } from 'next/navigation'
import { calculateLoan, createLoan } from "./services/moneyme";

import "react-datepicker/dist/react-datepicker.css";


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

export default function QuoteForm() {
  const router = useRouter()
  const [loading, setLoading] = useState<boolean>(false)
  const [startDate, setStartDate] = useState<Date | null>(null);

  const form = useForm<z.infer<typeof FormSchema>>({
    resolver: zodResolver(FormSchema),
    defaultValues: {
      term : 1,
      product: "",
      title: "",
      firstName: "",
      lastName: "",
      email: "",
      mobile: "",
      dateOfBirth: undefined,
      amountRequired: 100,
    },
  });

  const amountRequired = form.watch("amountRequired");
  const term = form.watch("term");

  async function onSubmit(data: z.infer<typeof FormSchema>) {
    setLoading(true)
  try {
    const loanParams = {
      term: data.term,
      product:  data.product,
      amountRequired: data.amountRequired
    }

    const response = await calculateLoan(loanParams)

    if(response.statusCode === 201)
    {
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
      const response = await createLoan(newData)
      
      if(response.statusCode === 201) {

        router.push(`/confirm/${response.result.id}`)
      }

      
    }

  
  } catch (error) {
    console.error("Error submitting data:", error);
    setLoading(false)
  }

  setLoading(false)

  }

  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-100">
      <div className="bg-white p-10 rounded-lg shadow-md w-full max-w-lg">
        <h1 className="text-center text-2xl font-bold mb-6">Quote Calculator</h1>

        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
            <div className="mb-6">
            <div>
            <label>Choose a Term</label>
            <Slider
                value={[term]}
                min={1}
                max={12}
                step={1}
                onValueChange={(value) => form.setValue("term", value[0])}
                className="w-full"
              />
              <div className="flex justify-between text-gray-600 mt-2">
                <span>1</span>
                <span className="font-semibold">{term} month</span>
                <span>12</span>
              </div>
            </div>

              <div className="mb-4">

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
                value={[amountRequired]}
                min={100}
                max={15000}
                step={100}
                onValueChange={(value) => form.setValue("amountRequired", value[0])}
                className="w-full"
              />
              <div className="flex justify-between text-gray-600 mt-2">
                <span>$100</span>
                <span className="font-semibold">${amountRequired}</span>
                <span>$15,000</span>
              </div>
            </div>

            <div className="flex space-x-4">
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
                  <FormItem className="flex-1">
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
                  <FormItem className="flex-1">
                    <FormControl>
                      <Input placeholder="Last name" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>

            <div className="flex space-x-4">
              <FormField
                control={form.control}
                name="email"
                render={({ field }) => (
                  <FormItem className="flex-1">
                    <FormControl>
                      <Input placeholder="Your email" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="mobile"
                render={({ field }) => (
                  <FormItem className="flex-1">
                    <FormControl>
                      <Input placeholder="Mobile number Philippines" {...field} maxLength={10} minLength={10} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>

            <FormField
              control={form.control}
              name="dateOfBirth"
              render={({ field }) => (
                <FormItem>
                  <FormControl>
                    <DatePicker
                      selected={startDate}
                      onChange={(date) => {
                        setStartDate(date);
                        field.onChange(date);
                      }}
                      showMonthDropdown
                      showYearDropdown
                      dropdownMode="select"
                      placeholderText="Date of Birth"
                      className="w-full px-3 py-2 border rounded-md"
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <Button
              type="submit"
              className="w-full bg-green-500 hover:bg-green-600 text-white font-bold py-3"
            >
              {loading ? <RefreshCwIcon className="animate-spin" /> : <></>}  &nbsp; Calculate Quote
            </Button>
          </form>
        </Form>

        <p className="text-center text-sm text-gray-500 mt-2">
          Quote does not affect your credit score
        </p>
      </div>
    </div>
  );
}
